#pragma strict

public var texture: Texture;
public var frame: float;

var center: Vector3;
var base_vector: Vector3;
var upper_right: float;
var upper_left: float;
var lower_left: float;
var lower_right: float;

public var icon_ratio_y = .77f;
public var icon_ratio_x = .95f;
public var scale = .01f;

function Start () {
	center = new Vector3(Screen.width/2f,Screen.height/2f,0);
	base_vector = new Vector3(Camera.main.pixelWidth,Camera.main.pixelHeight/2f,0) - center;
	upper_right = angle(base_vector,new Vector3(Camera.main.pixelWidth,Camera.main.pixelHeight,0) - center);
	upper_left = angle(base_vector, new Vector3(0,Camera.main.pixelHeight,0)-center);
	lower_left = angle(base_vector, new Vector3(0,0,0)-center);
	lower_right = angle(base_vector,new Vector3(Camera.main.pixelWidth,0,0)-center);
	
}

function Update () {
	//print(Camera.main.pixelWidth);
	//print(Camera.main.pixelHeight);
}

 //Two utility functions to find the intersection points between 2D lines and rectangles
 function Intersect(a1 : Vector2, a2 : Vector2, b1 : Vector2, b2 : Vector2) {
     var b : Vector2 = a2 - a1;
     var d : Vector2 = b2 - b1;
     var bDotDPerp : float = b.x * d.y - b.y * d.x;
 
     //If b dot d == 0, it means the lines are parallel so have infinite intersection points
     if(bDotDPerp == 0) {
         return null;
     }
 
     var c : Vector2 = b1 - a1;
     var t : float = (c.x * d.y - c.y * d.x) / bDotDPerp;
     if(t < 0 || t > 1) {
         return null;
     }
 
     var u : float = (c.x * b.y - c.y * b.x) / bDotDPerp;
     if (u < 0 || u > 1) {
         return null;
     }
 
     return a1 + t * b;
 }
 function Intersect(a : Vector2, b : Vector2, r : Rect) {
     var tl : Vector2 = Vector2(r.xMin, r.yMin);
     var bl : Vector2 = Vector2(r.xMin, r.yMax);
     var br : Vector2;
     var tr : Vector2;
 
     var i;
 
     i = Intersect(a, b, tl, bl); //Check left segment
     if(i != null) {
         return i;
     } else {
         br = Vector2(r.xMax, r.yMax);
         i = Intersect(a, b, bl, br); //Check bottom segment
         if(i != null) {
             return i;
         } else {
             tr = Vector2(r.xMax, r.yMin);
             i = Intersect(a, b, br, tr); //Check right segment
             if(i != null) {
                 return i;
             } else {
                 i = Intersect(a, b, tr, tl); //Check top segment
                 if(i != null) {
                     return i;
                 } else {
                     return null;
                 }
             }
         }
     }
 }
 
 //Now, this function is added to each of the objects that you want to show on screen
 function OnGUI() {
     var vp = Camera.main.WorldToViewportPoint(transform.position);
 
     if(vp.z > 0) {
         var ap : Vector2; //In viewport space
 
         if(vp.x >= 0 && vp.x <= 1 && vp.y >= 0 && vp.y <= 1) {
             return; //ap = vp;
         } else {
             ap = Intersect(Vector2.one / 2, vp, Rect(0, 0, 1, 1));
         }
 
         ap = Camera.main.ViewportToScreenPoint(ap);
         ap.y = Screen.height - ap.y;
         
         if (ap.x==0) ap.x=ap.x+frame;
         else if (ap.y==Screen.height) ap.y=ap.y-frame;
         else if (ap.x==Screen.width) ap.x=ap.x-frame;
         
         
         
//		var ang = angle(base_vector,Vector3(ap.x,ap.y,0)-center);
//	
//		if (ang<=upper_right && ang>lower_right){
//			print ("right");
//			ap.x = icon_ratio_x*Camera.main.pixelWidth;
//		}
//		else if (ang<=upper_left && ang>upper_right){
//			print ("top");
//			ap.y = (1-icon_ratio_y)*Camera.main.pixelHeight;
//		}
//		else if (ang<=lower_left || ang>upper_left){ //due to atan2 implementation
//			print ("left");
//			ap.x = (1-icon_ratio_x)*Camera.main.pixelWidth;
//		}
//		else if (ang<=lower_right && ang>lower_left){
//			print ("bottom");
//			ap.y = (icon_ratio_y)*Camera.main.pixelHeight;
//		}

//		if (ap.x>=icon_ratio_x*Screen.width) ap.x = icon_ratio_x*Screen.width;
//		if (ap.x<=(1-icon_ratio_x)*Screen.width) ap.x = (1-icon_ratio_x)*Screen.width;
//        if (ap.y>=icon_ratio_y*Screen.height) ap.y = icon_ratio_y*Screen.height;
//		if (ap.y<=(1-icon_ratio_y)*Screen.height) ap.y = (1-icon_ratio_y)*Screen.height;
        
        var current = Vector3(ap.x,ap.y,0);
        var target = Vector3(Screen.width/2,Screen.height/2,0);
        
        var p = Vector3.MoveTowards(current,target,20f);
        ap.x = p.x; ap.y = p.y;
        			
        print(ap);
         GUI.DrawTexture(Rect(ap.x, ap.y, 40, 40),texture);
         
         print(Screen.width*Screen.height*scale);
         //GUI.Box(Rect(ap.x,ap.y,5,5),".");
     }
     
 }
 
 function angle(b_v: Vector3, arm: Vector3){
		var dot_product = b_v.x*arm.x + b_v.y*arm.y;
		var determinant = b_v.x*arm.y - b_v.y*arm.x;
		return Mathf.Atan2(determinant,dot_product);
 }
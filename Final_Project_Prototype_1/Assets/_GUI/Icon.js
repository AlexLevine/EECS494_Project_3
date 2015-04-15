#pragma strict

public var texture: Texture;

function Start () {

}

function Update () {

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
         
         GUI.DrawTexture(Rect(ap.x - 75, ap.y - 75, 75, 75),texture);
     }
 }
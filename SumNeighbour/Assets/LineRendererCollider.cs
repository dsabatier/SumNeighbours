 using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 
 [RequireComponent(typeof(LineRenderer))]
 public class LineRendererCollider : MonoBehaviour
 {
     LineRenderer line;
     public void Start() {
         AddCollider(); //Increase the count of parts(6) if you want to get more detailed collider
     }
     public void AddCollider()
     {
         line = GetComponent<LineRenderer>();
         Vector3 start = line.GetPosition(0);
         Vector3 end = line.GetPosition(line.positionCount - 1);
         float length = Vector3.Distance(start, end);
         
         // create collider and set size
         BoxCollider collider = new GameObject("Collider").AddComponent<BoxCollider>();
         collider.transform.SetParent(transform);
         collider.size = new Vector3(length, 0.5f, 0.15f);

         // set center point
         Vector3 middle = (start + end) / 2;
         collider.transform.localPosition = middle;
         
         // match angle
         float angle = (Mathf.Abs(start.y - end.y) / Mathf.Abs(start.x - end.x));
         if ((start.y < end.y && start.x > end.x) || (end.y < start.y && end.x > start.x))
         {
             angle *= -1;
         }
         angle = Mathf.Rad2Deg * Mathf.Atan(angle);
         collider.transform.Rotate(0, 0, angle);

     }
 }
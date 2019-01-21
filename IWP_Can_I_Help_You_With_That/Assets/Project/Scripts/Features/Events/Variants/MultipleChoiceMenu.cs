using IWPCIH.EventTracking;
using System;
using UnityEngine;

namespace IWPCIH.TimelineEvents
{
    public class MultipleChoiceMenu : BaseEvent
    {
        public class MultipleChoiceData : TimelineEventData
        {
            public string Question = "";
            public string[] Answers = new string[0];
        }
        public override Type EventType { get { return typeof(MultipleChoiceData); } }


        public MultipleChoiceOption ChoicePrefab;
        public float Spacing;

        float x;
        float z;

        public override void Invoke()
        {

            (TimelineController.Instance as TimelineExecuter).TogglePause(true);

            LocateMenu();

            MultipleChoiceData myData = (MultipleChoiceData)Event;

            Spawn(-1, myData.Question);

            for (int i = 0; i < myData.Answers.Length; i++) 
            {
                string answer = myData.Answers[i];

                Spawn(i, answer);


            }
        }


        private void LocateMenu()
        {
            Vector3 lookDir = Camera.main.transform.forward; //= camera richting
            Quaternion lookRotation = Camera.main.transform.rotation;

            x = transform.eulerAngles.x;
            z = transform.eulerAngles.z;


            lookRotation = Quaternion.Euler(x, 0, z);

            transform.position = lookDir * 10f;
            transform.rotation = lookRotation;
        }


        private void Spawn(int index, string text)
        {
            MultipleChoiceOption option = Instantiate(ChoicePrefab, transform);
            option.transform.Translate(Vector3.down * index * Spacing);

            option.Parent = this;
          
            option.SetText(text);
           

            // lower object.
        }

       

        public void OnOptionSelected(MultipleChoiceOption selected)
        { 
            Destroy(ChoicePrefab, 5);
        }
        




    }
}

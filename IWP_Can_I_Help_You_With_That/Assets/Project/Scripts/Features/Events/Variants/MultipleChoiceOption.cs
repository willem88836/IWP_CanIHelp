using UnityEngine;


namespace IWPCIH.TimelineEvents
{
    public class MultipleChoiceOption :  MonoBehaviour, ISelectable
	{
        public TextMesh TextField;

        public MultipleChoiceMenu Parent = null;





        public void Select()
        {
            Parent.OnOptionSelected(this);
        }
        

        public void SetText(string text)
        {
            TextField.text = text;
        } 
	}
}

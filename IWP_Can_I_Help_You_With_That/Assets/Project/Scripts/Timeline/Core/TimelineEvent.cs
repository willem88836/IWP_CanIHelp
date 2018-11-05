namespace IWPCIH.EventTracking
{
	[System.Serializable]
	public class TimelineEvent
	{
		public enum EventType { CropStart };

		public float InvokeTime;
		public EventType Type;


		public TimelineEvent(float invokeTime, EventType type)
		{
			this.InvokeTime = invokeTime;
			this.Type = type;
		}
	}
}

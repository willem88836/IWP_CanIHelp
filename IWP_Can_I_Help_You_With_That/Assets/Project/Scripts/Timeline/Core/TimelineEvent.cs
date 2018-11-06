namespace IWPCIH.EventTracking
{
	[System.Serializable]
	public abstract class TimelineEvent
	{
		public float InvokeTime;
		public TimelineEvents.EventContainer.EventType Type;
		public int Id;
	}
}

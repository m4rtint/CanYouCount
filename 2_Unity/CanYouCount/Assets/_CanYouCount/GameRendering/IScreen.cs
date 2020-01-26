namespace CanYouCount
{
	public interface IScreen
	{
		void InitializeScreen(ApplicationManager appManager);

		void HideScreen(bool isInstant = false);
		void ShowScreen(bool isInstant = false);

		void UpdateScreen(float deltaTime);
	}
}

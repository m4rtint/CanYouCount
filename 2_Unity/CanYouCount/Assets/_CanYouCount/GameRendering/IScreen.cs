namespace CanYouCount
{
	public interface IScreen
	{
		void InitializeScreen(ApplicationManager appManager);

		void HideScreen();
		void ShowScreen();

		void UpdateScreen(float deltaTime);
	}
}

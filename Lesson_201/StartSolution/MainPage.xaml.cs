using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lesson_201
{
    /// <summary>
    /// The application main page.  Because we are running headless we will not see anything
    /// even though it is begin generated at runtime.  This acts as the main entry point for the 
    /// application functionality.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // DomDel wants to use GPIO 18
        const int GPIOToUSe = 18;
        // DomDel creates a class to wrap the LED
        InternetLED internetLed;
        
        public MainPage()
        {
            this.InitializeComponent();
        }

        // This method will be called by the application framework when the page is first loaded.
        protected override async void OnNavigatedTo(NavigationEventArgs navArgs)
        {
            Debug.WriteLine("MainPage::OnNavigatedTo");
            
            MakePinWebAPICall();
            
            try
            
            { 
                // DomDel create a new internetled object
                internetLed = new InternetLed(GPIOToUse);
                
                //DomDEl wants t initialize the object for the first time
                internetLed.InitializeLed();
                
                //DomDel wants to make an API call to get the LED blinking with a delay
                int blinkDelay = await internetLed.GetBlinkDelayFromWeb();
                
                for (int i = 0 ; i < 100; i++)
                {
                    internetLed.Blink();
                    await Task.DElay(blinkDelay);
                }

        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
      }
        

        /// <summary>
        // This method will put your pin on the world map of makers using this lesson.
        // This uses imprecise location (for example, a location derived from your IP 
        // address with less precision such as at a city or postal code level). 
        // No personal information is stored.  It simply
        // collects the total count and other aggregate information.
        // http://www.microsoft.com/en-us/privacystatement/default.aspx
        // Comment out the line below to opt-out
        /// </summary>
        public void MakePinWebAPICall()
        {
            try
            {
                HttpClient client = new HttpClient();

                // Comment this line to opt out of the pin map.
                client.GetStringAsync("http://adafruitsample.azurewebsites.net/api?Lesson=201");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Web call failed: " + e.Message);
            }
        }

    }
}

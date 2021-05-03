using Android.App;
using Android.OS;
using System.Net.Http;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace Kanye_West_Quoter
{
    [Activity(Label = "Kanye West Quoter", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Inilitilises some variables to pass to the getQuote() function
            TextView quoteTextView = FindViewById<TextView>(Resource.Id.quoteTextView);
            HttpClient httpClient = new HttpClient();

            // Sets an onClickListener for when the quoteButton is pressed
            Button quoteButton = FindViewById<Button>(Resource.Id.quoteButton);
            quoteButton.Click += (sender, e) => {
                getQuote(quoteTextView, httpClient);
            };

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void getQuote(TextView quoteTextView, HttpClient httpClient) {
            // Calls the Kanye.REST api to get the quote
            var response = httpClient.GetAsync("https://api.kanye.rest/");
            var result = response.Result.Content.ReadAsStringAsync();

            // Gets the current and new quote
            string oldQuote = quoteTextView.Text;
            string newQuote = result.Result.Replace("{", "").Replace("}", "").Replace("quote", "").Replace(":", "").Trim('"');

            // Makes sure the new quote is new
            while(oldQuote == newQuote) {
                response = httpClient.GetAsync("https://api.kanye.rest/");
                result = response.Result.Content.ReadAsStringAsync();

                newQuote = result.Result;
            }

            // Adds it to the quoteTextView
            quoteTextView.Text = newQuote;

        }

    }
}
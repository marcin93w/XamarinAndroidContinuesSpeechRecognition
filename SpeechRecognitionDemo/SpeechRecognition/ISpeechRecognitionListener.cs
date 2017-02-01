using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;
using Java.Security;

namespace SpeechRecognitionDemo.SpeechRecognition
{
    public interface ISpeechRecognitionListener
    {
        void OnSpeechRecognitionError(SpeechRecognizerError error);
        void OnSpeechRecognitionResults(IEnumerable<string> results);
    }
}
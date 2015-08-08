using Windows.ApplicationModel.Activation;
using GoogleLogin;

namespace GoogleLogin.Common
{
    /// <summary>
    /// ContinuationManager is used to detect if the most recent activation was due to a
    /// authentication continuation.
    ///
    /// Note: To keep this sample as simple as possible, the content of the file was changed to support
    /// WebAuthenticationBrokerContinuation ONLY.
    /// Take a look in http://msdn.microsoft.com/en-us/library/dn631755.aspx
    /// for a full documentation on how to support continuation in other cases.
    /// </summary>
    public class ContinuationManager
    {
        /// <summary>
        /// Sets the ContinuationArgs for this instance.
        /// Should be called by the main activation handling code in App.xaml.cs.
        /// </summary>
        /// <param name="args">The activation args.</param>
        internal void Continue(IContinuationActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.WebAuthenticationBrokerContinuation)
            {
                var page = MainPage.Current as IWebAuthenticationContinuable;
                page?.ContinueWebAuthentication(
                    args as WebAuthenticationBrokerContinuationEventArgs);
            }
        }
    }

    /// <summary>Implement this interface if your page invokes the web authentication broker.</summary>
    interface IWebAuthenticationContinuable
    {
        /// <summary>
        /// This method is invoked when the web authentication broker returns with the authentication result.
        /// </summary>
        /// <param name="args">Activated event args object that contains returned authentication token.</param>
        void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args);
    }
}
package crc64d693e2d9159537db;


public class BlazorWebViewHandler_BlazorWebViewPredictiveBackCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.window.OnBackInvokedCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onBackInvoked:()V:GetOnBackInvokedHandler:Android.Window.IOnBackInvokedCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebViewHandler+BlazorWebViewPredictiveBackCallback, Microsoft.AspNetCore.Components.WebView.Maui", BlazorWebViewHandler_BlazorWebViewPredictiveBackCallback.class, __md_methods);
	}

	public BlazorWebViewHandler_BlazorWebViewPredictiveBackCallback ()
	{
		super ();
		if (getClass () == BlazorWebViewHandler_BlazorWebViewPredictiveBackCallback.class) {
			mono.android.TypeManager.Activate ("Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebViewHandler+BlazorWebViewPredictiveBackCallback, Microsoft.AspNetCore.Components.WebView.Maui", "", this, new java.lang.Object[] {  });
		}
	}

	public void onBackInvoked ()
	{
		n_onBackInvoked ();
	}

	private native void n_onBackInvoked ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}

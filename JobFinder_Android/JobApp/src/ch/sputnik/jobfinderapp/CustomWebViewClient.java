/**
 *  Android JobFinderApp wrapper
 *  ©2012 Sputnik Informatik GmbH
 */

package ch.sputnik.jobfinderapp;

import android.app.Activity;
import android.content.Intent;
import android.net.MailTo;
import android.net.Uri;
import android.webkit.WebView;
import android.webkit.WebViewClient;

/**
 * Custom WebVieClient class to handle special urls (mailto, tel).
 */
public class CustomWebViewClient extends WebViewClient {
	
	/**
	 * Activity context 
	 */
	private Activity m_Context;

	/***
	 * The constructor.
	 * @param context The activity context.
	 */
	public CustomWebViewClient(Activity context) {
		this.m_Context = context;
	}

	/**
	 * Called when a new url is loaded.
	 */
	@Override
	public boolean shouldOverrideUrlLoading(WebView view, String url) {
		if (url.startsWith("mailto:")) {
			// Handle "mailto" links
			MailTo mt = MailTo.parse(url);
			Intent i = new Intent(Intent.ACTION_SEND);
			i.setType("text/plain");
			i.putExtra(Intent.EXTRA_EMAIL, new String[] { mt.getTo() });
			i.putExtra(Intent.EXTRA_SUBJECT, mt.getSubject());
			i.putExtra(Intent.EXTRA_CC, mt.getCc());
			i.putExtra(Intent.EXTRA_TEXT, mt.getBody());
			m_Context.startActivity(i);
			return true;
		} else if (url.startsWith("tel:")) {
			// Handle "tel" links
			Intent intent = new Intent(Intent.ACTION_DIAL, Uri.parse(url));
			m_Context.startActivity(intent);
			return true;
		}
		view.loadUrl(url);
		return true;
	}
}

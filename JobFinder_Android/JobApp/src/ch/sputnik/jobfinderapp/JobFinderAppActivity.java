/**
 *  Android JobFinderApp wrapper
 *  ©2012 Sputnik Informatik GmbH
 */

package ch.sputnik.jobfinderapp;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.res.Configuration;
import android.graphics.Bitmap;
import android.os.Bundle;
import android.view.ViewGroup;
import android.view.ViewGroup.LayoutParams;
import android.webkit.WebView;
import android.widget.FrameLayout;

/**
 * JobFinderApp Activity
 */
public class JobFinderAppActivity extends Activity {

	/**
	 * The placeholder control for the webview.
	 */
	private FrameLayout m_WebViewPlaceholder;

	/**
	 * The web view control.
	 */
	private WebView m_WebView;

	/**
	 * Called when the activity is first created.
	 */
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.main);

		initUI();
	}

	/**
	 * Initializes the UI.
	 */
	private void initUI() {

		m_WebViewPlaceholder = ((FrameLayout) findViewById(R.id.webViewPlaceholder));

		if (m_WebView == null) {
			m_WebView = new WebView(this);
			m_WebView.setLayoutParams(new ViewGroup.LayoutParams(LayoutParams.FILL_PARENT, LayoutParams.FILL_PARENT));
			m_WebView.setScrollBarStyle(WebView.SCROLLBARS_OUTSIDE_OVERLAY);
			m_WebView.setScrollbarFadingEnabled(true);
			m_WebView.getSettings().setLoadsImagesAutomatically(true);
			m_WebView.getSettings().setJavaScriptEnabled(true);

			// Set custom web view client to handle special urls (mailto, tel,...)
			CustomWebViewClient customWebViewClient = new CustomWebViewClient(this) {

				/**
				 * Called when a page is about to be loaded.
				 */
				@Override
				public void onPageStarted(WebView view, String url, Bitmap favicon) {
					super.onPageStarted(view, url, favicon);
					
					handlePageStartLoading(view, url, favicon);
				}
				
				/**
				 * Called when a pages has been loaded.
				 */
				@Override
				public void onPageFinished(WebView view, String url) {
					super.onPageFinished(view, url);

					handlePageFinished(view, url);

				}
			};
			
			m_WebView.setWebViewClient(customWebViewClient);

			// Retrieve the URL that should be loaded:
			// a) from settings (preferences).
			// b) from resource file if no setting is present (the app starts for the first time).
			SharedPreferences preferences = getSharedPreferences(Constants.SETTINGS_FILE_NAME, MODE_PRIVATE);
			String urlToLoad = preferences.getString(Constants.SPUTNIK_APP_URL, this.getString(R.string.jobFinderAppCreateURL));

			// Load the URLs inside the WebView, not in the external web browser.
			m_WebView.loadUrl(urlToLoad);

		}

		// Attach the WebView to its placeholder
		m_WebViewPlaceholder.addView(m_WebView);
	}

	/**
	 * Called when the configuration changes.
	 */
	@Override
	public void onConfigurationChanged(Configuration newConfig) {
		
		if (m_WebView != null) {
			// Remove the web view from the old placeholder.
			m_WebViewPlaceholder.removeView(m_WebView);
		}

		super.onConfigurationChanged(newConfig);

		// Load the layout resource for the new configuration
		setContentView(R.layout.main);

		// Reinitialize the UI
		initUI();
	}
	
	/**
	 * Handles page load start event.
	 * @param view The web view.
	 * @param url The URL that is loaded.
	 * @param favicon The favorite icon.
	 */
	private void handlePageStartLoading(WebView view, String url, Bitmap favicon) {
		// Display loading indication in title bar.
		setTitle(getString(R.string.loading));
	}

	/**
	 * Handles page load finished event.
	 * @param view The we bview.
	 * @param url The URL that has been loaded.
	 */
	private void handlePageFinished(WebView view, String url) {
		
		// Save the loaded URL in the settings file if not already done...
		
		SharedPreferences preferences = getSharedPreferences(Constants.SETTINGS_FILE_NAME, Context.MODE_PRIVATE);
		
		String sputnikAppUrlInSetting = preferences.getString(Constants.SPUTNIK_APP_URL, "");

		// Check if the sputnik app url is not set.
		if (sputnikAppUrlInSetting.equals("")) {
			String sputnikDomain = getString(R.string.sputnikDomain);

			// Only store URLS in the sputnik domain which do not match with the app create URL.
			if (url.startsWith(sputnikDomain) && !url.equals(getString(R.string.jobFinderAppCreateURL))) {

				SharedPreferences.Editor editor = preferences.edit();
				// Save the url of the web view.
				editor.putString(Constants.SPUTNIK_APP_URL, url);
				editor.commit();	
			}
		}
		
		// Show app name in title bar after a page has been loaded.
		setTitle(this.getString(R.string.app_name));
	}
}
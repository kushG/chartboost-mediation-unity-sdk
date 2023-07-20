package com.chartboost.mediation.unity

import android.app.Activity
import android.graphics.Color
import android.util.DisplayMetrics
import android.util.Log
import android.view.Gravity
import android.view.View
import android.view.ViewGroup
import com.chartboost.heliumsdk.ad.HeliumBannerAd
import com.unity3d.player.UnityPlayer

class BannerAdWrapper(private val ad: HeliumBannerAd) {

    private var bannerLayout: BannerLayout? = null
    private val activity: Activity? = UnityPlayer.currentActivity

    fun load() {
        runTaskOnUiThread {
            ad.load()
        }
    }

    fun load(screenLocation: Int) {
        runTaskOnUiThread {
            createBannerLayout(screenLocation)
            load()
        }
    }

    fun load(x: Float, y: Float, width: Int, height: Int) {
        runTaskOnUiThread {
            createBannerLayout(x,y,width,height)
            load()
        }
    }

    fun setKeyword(keyword: String, value: String): Boolean {
        return ad.keywords.set(keyword, value)
    }

    fun removeKeyword(keyword: String): String {
        return ad.keywords.remove(keyword) ?: ""
    }

    fun setBannerVisibility(isVisible: Boolean) {
        runTaskOnUiThread {
            if (bannerLayout != null) {
                val visibility = if (isVisible) View.VISIBLE else View.INVISIBLE
                bannerLayout?.visibility = visibility
                ad.visibility = visibility
            }
        }
    }

    fun clearLoaded() {
        runTaskOnUiThread {
            ad.clearAd()
        }
    }

    fun destroy() {
        runTaskOnUiThread {
            destroyBannerLayout()
            ad.destroy()
        }
    }

    fun enableDrag(dragListener:IBannerDragEventListener?){
        Log.d(TAG, "enableDrag");
        runTaskOnUiThread {
            bannerLayout?.enableDrag = true;
            bannerLayout?.dragListener = dragListener;
        }
    }

    fun disableDrag(){
        Log.d(TAG, "disableDrag");
        runTaskOnUiThread {
            bannerLayout?.enableDrag = false;
        }
    }

    private fun createBannerLayout(screenLocation: Int){
        if (activity == null) {
            Log.w(TAG, "Activity not found")
            return
        }

        var layout = bannerLayout

        // Create the banner layout on the given position.
        // Check if there is an already existing banner layout. If so, remove it. Otherwise,
        // create a new one.

        layout?.let {
            it.removeAllViews()
            val bannerParent = it.parent as ViewGroup
            bannerParent.removeView(it)
        }

        layout = BannerLayout(activity, ad)
        layout.setBackgroundColor(Color.TRANSPARENT)

        /*
            //     TopLeft = 0,
            //     TopCenter = 1,
            //     TopRight = 2,
            //     Center = 3,
            //     BottomLeft = 4,
            //     BottomCenter = 5,
            //     BottomRight = 6
        */

        val bannerGravityPosition = when (screenLocation) {
            0 -> Gravity.TOP or Gravity.LEFT
            1 -> Gravity.TOP or Gravity.CENTER_HORIZONTAL
            2 -> Gravity.TOP or Gravity.RIGHT
            3 -> Gravity.CENTER_VERTICAL or Gravity.CENTER_HORIZONTAL
            4 -> Gravity.BOTTOM or Gravity.LEFT
            5 -> Gravity.BOTTOM or Gravity.CENTER_HORIZONTAL
            6 -> Gravity.BOTTOM or Gravity.RIGHT
            // Other cases
            else -> Gravity.TOP or Gravity.CENTER_HORIZONTAL
        }

        layout.gravity = bannerGravityPosition

        // Attach the banner layout to the activity.
        val density = displayDensity
        try {
            when (ad.getSize() ?: HeliumBannerAd.HeliumBannerSize.STANDARD) {
                HeliumBannerAd.HeliumBannerSize.LEADERBOARD -> ad.layoutParams = getBannerLayoutParams(density,LEADERBOARD.first, LEADERBOARD.second)
                HeliumBannerAd.HeliumBannerSize.MEDIUM -> ad.layoutParams = getBannerLayoutParams(density, MEDIUM.first, MEDIUM.second)
                HeliumBannerAd.HeliumBannerSize.STANDARD -> ad.layoutParams = getBannerLayoutParams(density, STANDARD.first, STANDARD.second)
            }

            // Attach the banner to the banner layout.
            layout.addView(ad)
            activity.addContentView(layout, ViewGroup.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT))

            // This immediately sets the visibility of this banner. If this doesn't happen
            // here, it is impossible to set the visibility later.
            layout.visibility = View.VISIBLE

            // This affects future visibility of the banner layout. Despite it never being
            // set invisible, not setting this to visible here makes the banner not visible.
            layout.visibility = View.VISIBLE
        } catch (ex: Exception) {
            Log.w(TAG, "Helium encountered an error calling banner load() - ${ex.message}")
        }
        bannerLayout = layout
    }

    private fun createBannerLayout(x: Float, y: Float, width: Int, height: Int) {
        if (activity == null) {
            Log.w(TAG, "Activity not found")
            return
        }

        var layout = bannerLayout

        // Create the banner layout on the given position.
        // Check if there is an already existing banner layout. If so, remove it. Otherwise,
        // create a new one.

        layout?.let {
            it.removeAllViews()
            val bannerParent = it.parent as ViewGroup
            bannerParent.removeView(it)
        }

        layout = BannerLayout(activity, ad)
        layout.setBackgroundColor(Color.TRANSPARENT)

        ad.layoutParams = ViewGroup.LayoutParams(width, height)
        ad.x = x;
        ad.y = y;

        try {
            // Attach the banner to the banner layout.
            layout.addView(ad)
            activity.addContentView(layout, ViewGroup.LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT))

            // This immediately sets the visibility of this banner. If this doesn't happen
            // here, it is impossible to set the visibility later.
            layout.visibility = View.VISIBLE

            // This affects future visibility of the banner layout. Despite it never being
            // set invisible, not setting this to visible here makes the banner not visible.
            layout.visibility = View.VISIBLE
        } catch (ex: Exception) {
            Log.w(TAG, "Helium encountered an error calling banner load() - ${ex.message}")
        }

        bannerLayout = layout
    }

    private fun destroyBannerLayout(){
        bannerLayout?.let {
            it.removeAllViews()
            it.visibility = View.GONE
        }
    }

    private fun getBannerLayoutParams(pixels: Float, width: Int, height: Int): ViewGroup.LayoutParams {
        return ViewGroup.LayoutParams((pixels * width).toInt(), (pixels * height).toInt())
    }

    private val displayDensity: Float
        get() {
            return activity?.resources?.displayMetrics?.density ?: DisplayMetrics.DENSITY_DEFAULT.toFloat()
        }


    companion object {
        private val TAG = BannerAdWrapper::class.java.simpleName
        private val STANDARD = Pair(320, 50)
        private val MEDIUM = Pair(300, 250)
        private val LEADERBOARD = Pair(728, 90)

        @JvmStatic
        fun wrap(ad: HeliumBannerAd): BannerAdWrapper {
            return BannerAdWrapper(ad)
        }

        fun runTaskOnUiThread(runnable: Runnable) {
            UnityPlayer.currentActivity.runOnUiThread {
                try {
                    runnable.run()
                } catch (ex: Exception) {
                    Log.w(TAG, "Exception found when running on UI Thread: ${ex.message}")
                }
            }
        }
    }
}
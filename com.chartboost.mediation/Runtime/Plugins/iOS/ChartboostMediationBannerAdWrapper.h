//
//  ChartboostMediationBannerAdWrapper.h
//  Unity-iPhone
//
//  Created by Kushagra Gupta on 6/21/23.
//

#ifndef ChartboostMediationBannerAdWrapper_h
#define ChartboostMediationBannerAdWrapper_h



#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <ChartboostMediationSDK/ChartboostMediationSDK-Swift.h>


typedef void (*ChartboostMediationBannerDragEvent)(void* uniqueId, float x, float y);


@interface ChartboostMediationBannerAdWrapper : NSObject

@property HeliumBannerView* bannerView;
@property ChartboostMediationBannerDragEvent dragListener;

- (instancetype)initWithBannerView: (HeliumBannerView*) bannerView;
- (void)enableDragWithDragListener:(ChartboostMediationBannerDragEvent) dragListener;
- (void)disableDrag;


@end
#endif /* ChartboostMediationBannerAdWrapper_h */

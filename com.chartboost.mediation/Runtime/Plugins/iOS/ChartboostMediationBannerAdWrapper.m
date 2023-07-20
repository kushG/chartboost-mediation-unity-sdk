//
//  ChartboostMediationBannerAdWrapper.m
//  Unity-iPhone
//
//  Created by Kushagra Gupta on 6/21/23.
//

#import "ChartboostMediationBannerAdWrapper.h"


@implementation ChartboostMediationBannerAdWrapper

- (instancetype)initWithBannerView:(HeliumBannerView *)bannerView {
    self.bannerView = bannerView;
    return self;
}

- (void)enableDragWithDragListener:(ChartboostMediationBannerDragEvent)dragListener {
    
    UIPanGestureRecognizer *panGesture = [[UIPanGestureRecognizer alloc] initWithTarget:self action:@selector(handlePan:)];
    [self.bannerView addGestureRecognizer:panGesture];
        
    self.dragListener = dragListener;
}

- (void)disableDrag {
    self.dragListener = NULL;
}

- (void)handlePan:(UIPanGestureRecognizer *)gr
{
    CGPoint translation = [gr translationInView:gr.view.superview];
    CGPoint center = gr.view.center;
    center.x += translation.x;
    center.y += translation.y;
    gr.view.center = center;
    [gr setTranslation:CGPointZero inView:gr.view.superview];
        
    float scale = UIScreen.mainScreen.scale;
    float x = gr.view.frame.origin.x * scale;
    float y = gr.view.frame.origin.y * scale;
    
    if(self.dragListener != nil)
    {
        self.dragListener((__bridge void*)self, x, y);
    }
}

@end
#ifndef CUSTOM_RIMLIGHTING_INCLUDED
#define CUSTOM_RIMLIGHTING_INCLUDED

void AdditionalLightsAngle_float(float3 WorldNormal, float3 WorldPos, out float cosAngle){
    WorldNormal = normalize(WorldNormal);
        cosAngle = 1;
        #ifndef SHADERGRAPH_PREVIEW

    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPos);
        cosAngle = min( dot(WorldNormal, light.direction) * light.distanceAttenuation, cosAngle);
    }   
     float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);

     Light mainLight = GetMainLight(shadowCoord);
     cosAngle = min(dot(WorldNormal, mainLight.direction) * mainLight.distanceAttenuation, cosAngle);

     #endif

    
   /* Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    ShadowAtten = mainLight.shadowAttenuation;
    */

}

void AdditionalLightsAngle_half(half3 WorldNormal, half3 WorldPos, out half cosAngle){
    WorldNormal = normalize(WorldNormal);
        cosAngle = 0;

        #ifndef SHADERGRAPH_PREVIEW

    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPos);
        cosAngle = min( dot(WorldNormal, light.direction), cosAngle);
    }   
     float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);

     Light mainLight = GetMainLight(shadowCoord);
         cosAngle = min(dot(WorldNormal, mainLight.direction), cosAngle);

         #endif

  
   /* Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    ShadowAtten = mainLight.shadowAttenuation;
    */

}
#endif

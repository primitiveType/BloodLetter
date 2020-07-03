#ifndef CUSTOM_RIMLIGHTING_INCLUDED
#define CUSTOM_RIMLIGHTING_INCLUDED
float TakeClosestToValue(float first, float second, float value)
{
    float diffFirst = value - first;
    float diffSecond = value - second;
    if(abs(diffFirst) > abs(diffSecond))
    {
       return second;
    }
    return first;
}
void AdditionalLightsAngle_float(float3 WorldNormal, float3 WorldPos, out float cosAngle){
    WorldNormal = normalize(WorldNormal);
        cosAngle = 1;
        #ifndef SHADERGRAPH_PREVIEW

    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPos);
        float tempCos = dot(TransformWorldToObject(WorldNormal), TransformWorldToObject(light.direction));
        cosAngle = TakeClosestToValue(tempCos, cosAngle, -.5);
   
    }   
     float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);

     Light mainLight = GetMainLight(shadowCoord);
     cosAngle = TakeClosestToValue(dot(TransformWorldToObject(WorldNormal), TransformWorldToObject(mainLight.direction)) , cosAngle, -.5);

     #endif

   /* Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    ShadowAtten = mainLight.shadowAttenuation;
    */

}

void OutputLightDirection_float(float3 WorldPos, out float3 direction){
direction = float3(0,0,0);
        #ifndef SHADERGRAPH_PREVIEW

 int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPos);
        direction = light.direction;
   
    }    
    
    #endif
}



void AdditionalLightsAngle_half(half3 WorldNormal, half3 WorldPos, out half cosAngle){
cosAngle = 1;
 /*   WorldNormal = normalize(WorldNormal);
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

  */
   /* Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    ShadowAtten = mainLight.shadowAttenuation;
    */

}
#endif

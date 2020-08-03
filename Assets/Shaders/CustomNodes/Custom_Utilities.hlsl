#ifndef MY_HLSL_UTILITIES
#define MY_HLSL_UTILITIES

#pragma region MIN
void Min3V1_float (float a, float b, float c, out float minValue) {
    minValue = min( min(a, b), c );
}
void Min3V2_float (float2 a, float2 b, float2 c, out float2 minValue) {
    minValue = min( min(a, b), c );
}
void Min3V3_float (float3 a, float3 b, float3 c, out float3 minValue) {
    minValue = min( min(a, b), c );
}
void Min3V4_float (float4 a, float4 b, float4 c, out float4 minValue) {
    minValue = min( min(a, b), c );
}
void Min4V1_float (float a, float b, float c, float d, out float minValue) {
    minValue = min( min(a, b), min(c, d) );
}
void Min4V2_float (float2 a, float2 b, float2 c, float2 d, out float2 minValue) {
    minValue = min( min(a, b), min(c, d) );
}
void Min4V3_float (float3 a, float3 b, float3 c, float3 d, out float3 minValue) {
    minValue = min( min(a, b), min(c, d) );
}
void Min4V4_float (float4 a, float4 b, float4 c, float4 d, out float4 minValue) {
    minValue = min( min(a, b), min(c, d) );
}
#pragma endregion // MIN


#pragma region MAX
void Max3V1_float (float a, float b, float c, out float maxValue) {
    maxValue = max( max(a, b), c );
}
void Max3V2_float (float2 a, float2 b, float2 c, out float2 maxValue) {
    maxValue = max( max(a, b), c );
}
void Max3V3_float (float3 a, float3 b, float3 c, out float3 maxValue) {
    maxValue = max( max(a, b), c );
}
void Max3V4_float (float4 a, float4 b, float4 c, out float4 maxValue) {
    maxValue = max( max(a, b), c );
}
void Max4V1_float (float a, float b, float c, float d, out float maxValue) {
    maxValue = max( max(a, b), max(c, d) );
}
void Max4V2_float (float2 a, float2 b, float2 c, float2 d, out float2 maxValue) {
    maxValue = max( max(a, b), max(c, d) );
}
void Max4V3_float (float3 a, float3 b, float3 c, float3 d, out float3 maxValue) {
    maxValue = max( max(a, b), max(c, d) );
}
void Max4V4_float (float4 a, float4 b, float4 c, float4 d, out float4 maxValue) {
    maxValue = max( max(a, b), max(c, d) );
}
#pragma endregion // MAX


#pragma region BOOLEAN TO FLOAT
void BoolToV1_float (bool x, out float f) {
    f = float(x);
}
void BoolToV2_float (bool x, bool y, out float2 f) {
    f = float2(x, y);
}
void BoolToV3_float (bool x, bool y, bool z, out float3 f) {
    f = float3(x, y, z);
}
void BoolToV4_float (bool x, bool y, bool z, bool w, out float4 f) {
    f = float4(x, y, z, w);
}
#pragma endregion // BOOLEAN TO FLOAT

#endif // MY_HLSL_UTILITIES
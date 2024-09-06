Shader "Custom/WalriderShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _DissolveThreshold ("Dissolve Threshold", Range(0, 1)) = 0.5
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _MainTex;
        float _DissolveThreshold;
        fixed4 _EmissionColor;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            fixed noise = tex2D(_MainTex, IN.uv_MainTex).r;
            c.a = step(noise, _DissolveThreshold);

            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Emission = _EmissionColor.rgb * c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

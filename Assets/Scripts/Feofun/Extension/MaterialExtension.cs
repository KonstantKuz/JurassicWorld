using UnityEngine;
using UnityEngine.Rendering;

namespace Feofun.Extension
{
    public static class MaterialExtension
    {
        private const string SRC_BLEND = "_SrcBlend";
        private const string DST_BLEND = "_DstBlend";
        private const string Z_WRITE = "_ZWrite";
        private const string ALPHATEST_ON = "_ALPHATEST_ON";
        private const string ALPHA_BLEND_ON = "_ALPHABLEND_ON";
        private const string ALPHA_PREMULTIPLY_ON = "_ALPHAPREMULTIPLY_ON";

        public static void ToTransparent(this Material material)
        {
            material.SetInt(SRC_BLEND, (int) BlendMode.One);
            material.SetInt(DST_BLEND, (int) BlendMode.OneMinusSrcAlpha);
            material.SetInt(Z_WRITE, 0);
            material.DisableKeyword(ALPHATEST_ON);
            material.DisableKeyword(ALPHA_BLEND_ON);
            material.EnableKeyword(ALPHA_PREMULTIPLY_ON);
            material.renderQueue = 3000;
        }
    }
}
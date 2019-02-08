using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DefferedFix : MonoBehaviour {

	public Material material;
	private RenderTexture buffer;
	
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		buffer = RenderTexture.GetTemporary(source.width, source.height, 24);

		Graphics.SetRenderTarget (buffer.colorBuffer, source.depthBuffer);

		Graphics.Blit(source, buffer, material);

		Graphics.Blit(buffer, destination);

		//RenderTexture.ReleaseTemporary(buffer);
	}

	void OnPostRender(){
		RenderTexture.ReleaseTemporary (buffer);
	}
}

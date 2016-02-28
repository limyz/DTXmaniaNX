#include "stdafx.h"
/*
* Copyright (c) 2007-2012 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

#include <d3d10.h>

#include "../Utilities.h"

#include "StateBlockMask.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Collections::ObjectModel;

namespace SlimDX
{
namespace Direct3D10
{
	StateBlockMask::StateBlockMask()
	{
		m_VSSamplers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_SAMPLER_SLOT_COUNT ) );
		m_VSShaderResources = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT ) );
		m_VSConstantBuffers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT ) );
		m_GSSamplers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_SAMPLER_SLOT_COUNT ) );
		m_GSShaderResources = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT ) );
		m_GSConstantBuffers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT ) );
		m_PSSamplers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_SAMPLER_SLOT_COUNT ) );
		m_PSShaderResources = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT ) );
		m_PSConstantBuffers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT ) );
		m_IAVertexBuffers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_IA_VERTEX_INPUT_RESOURCE_SLOT_COUNT ) );
	}

	StateBlockMask::StateBlockMask( StateBlockMask^ other )
	{
		m_VS = other->m_VS;
		m_VSSamplers = gcnew List<bool>( m_VSSamplers );
		m_VSShaderResources = gcnew List<bool>( other->m_VSShaderResources );
		m_VSConstantBuffers = gcnew List<bool>( other->m_VSConstantBuffers );
		m_GS = other->m_GS;
		m_GSSamplers = gcnew List<bool>( m_GSSamplers );
		m_GSShaderResources = gcnew List<bool>( other->m_GSShaderResources );
		m_GSConstantBuffers = gcnew List<bool>( other->m_GSConstantBuffers );
		m_PS = other->m_PS;
		m_PSSamplers = gcnew List<bool>( other->m_PSSamplers );
		m_PSShaderResources = gcnew List<bool>( other->m_PSShaderResources );
		m_PSConstantBuffers = gcnew List<bool>( other->m_PSConstantBuffers );
		m_IAVertexBuffers = gcnew List<bool>( other->m_IAVertexBuffers );
		m_IAIndexBuffer = other->m_IAIndexBuffer;
		m_IAInputLayout = other->m_IAInputLayout;
		m_IAPrimitiveTopology = other->m_IAPrimitiveTopology;
		m_OMRenderTargets = other->m_OMRenderTargets;
		m_OMDepthStencilState = other->m_OMDepthStencilState;
		m_OMBlendState = other->m_OMBlendState;
		m_RSViewports = other->m_RSViewports;
		m_RSScissorRects = other->m_RSScissorRects;
		m_RSRasterizerState = other->m_RSRasterizerState;
		m_SOBuffers = other->m_SOBuffers;
		m_Predication = other->m_Predication;
	}
	
	StateBlockMask::StateBlockMask( const D3D10_STATE_BLOCK_MASK& native )
	{
		m_VS = native.VS ? true : false;
		m_GS = native.GS ? true : false;
		m_PS = native.PS ? true : false;
		m_IAIndexBuffer = native.IAIndexBuffer ? true : false;
		m_IAInputLayout = native.IAInputLayout ? true : false;
		m_IAPrimitiveTopology = native.IAPrimitiveTopology ? true : false;
		m_OMRenderTargets = native.OMRenderTargets ? true : false;
		m_OMDepthStencilState = native.OMDepthStencilState ? true : false;
		m_OMBlendState = native.OMBlendState ? true : false;
		m_RSViewports = native.RSViewports ? true : false;
		m_RSScissorRects = native.RSScissorRects ? true : false;
		m_RSRasterizerState = native.RSRasterizerState ? true : false;
		m_SOBuffers = native.SOBuffers ? true : false;
		m_Predication = native.Predication ? true : false;
		
		m_VSSamplers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_SAMPLER_SLOT_COUNT ) );
		for( int index = 0; index < m_VSSamplers->Capacity; ++index )
		{
			m_VSSamplers[index] = native.VSSamplers[index] ? true : false;
		}
		
		m_VSShaderResources = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT ) );
		for( int index = 0; index < m_VSShaderResources->Capacity; ++index )
		{
			m_VSShaderResources[index] = native.VSShaderResources[index] ? true : false;
		}
		
		m_VSConstantBuffers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT ) );
		for( int index = 0; index < m_VSConstantBuffers->Capacity; ++index )
		{
			m_VSConstantBuffers[index] = native.VSConstantBuffers[index] ? true : false;
		}
		
		m_GSSamplers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_SAMPLER_SLOT_COUNT ) );
		for( int index = 0; index < m_GSSamplers->Capacity; ++index )
		{
			m_GSSamplers[index] = native.GSSamplers[index] ? true : false;
		}
		
		m_GSShaderResources = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT ) );
		for( int index = 0; index < m_GSShaderResources->Capacity; ++index )
		{
			m_GSShaderResources[index] = native.GSShaderResources[index] ? true : false;
		}
		
		m_GSConstantBuffers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT ) );
		for( int index = 0; index < m_GSConstantBuffers->Capacity; ++index )
		{
			m_GSConstantBuffers[index] = native.GSConstantBuffers[index] ? true : false;
		}
		
		m_PSSamplers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_SAMPLER_SLOT_COUNT ) );
		for( int index = 0; index < m_PSSamplers->Capacity; ++index )
		{
			m_PSSamplers[index] = native.PSSamplers[index] ? true : false;
		}
		
		m_PSShaderResources = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_INPUT_RESOURCE_SLOT_COUNT ) );
		for( int index = 0; index < m_PSShaderResources->Capacity; ++index )
		{
			m_PSShaderResources[index] = native.PSShaderResources[index] ? true : false;
		}
		
		m_PSConstantBuffers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_COMMONSHADER_CONSTANT_BUFFER_API_SLOT_COUNT ) );
		for( int index = 0; index < m_PSConstantBuffers->Capacity; ++index )
		{
			m_PSConstantBuffers[index] = native.PSConstantBuffers[index] ? true : false;
		}
		
		m_IAVertexBuffers = gcnew List<bool>( D3D10_BYTES_FROM_BITS( D3D10_IA_VERTEX_INPUT_RESOURCE_SLOT_COUNT ) );
		for( int index = 0; index < m_IAVertexBuffers->Capacity; ++index )
		{
			m_IAVertexBuffers[index] = native.IAVertexBuffers[index] ? true : false;
		}
	}
	
	D3D10_STATE_BLOCK_MASK StateBlockMask::CreateNativeVersion()
	{
		D3D10_STATE_BLOCK_MASK native;
		
		native.VS = m_VS;
		native.GS = m_GS;
		native.PS = m_PS;
		native.IAIndexBuffer = m_IAIndexBuffer;
		native.IAInputLayout = m_IAInputLayout;
		native.IAPrimitiveTopology = m_IAPrimitiveTopology;
		native.OMRenderTargets = m_OMRenderTargets;
		native.OMDepthStencilState = m_OMDepthStencilState;
		native.OMBlendState = m_OMBlendState;
		native.RSViewports = m_RSViewports;
		native.RSScissorRects = m_RSScissorRects;
		native.RSRasterizerState = m_RSRasterizerState;
		native.SOBuffers = m_SOBuffers;
		native.Predication = m_Predication;
		
		for( int index = 0; index < m_VSSamplers->Count; ++index )
		{
			native.VSSamplers[index] = m_VSSamplers[index];
		}
		
		for( int index = 0; index < m_VSShaderResources->Count; ++index )
		{
			native.VSShaderResources[index] = m_VSShaderResources[index];
		}
		
		for( int index = 0; index < m_VSConstantBuffers->Count; ++index )
		{
			native.VSConstantBuffers[index] = m_VSConstantBuffers[index];
		}
		
		for( int index = 0; index < m_GSSamplers->Count; ++index )
		{
			native.GSSamplers[index] = m_GSSamplers[index];
		}
		
		for( int index = 0; index < m_GSShaderResources->Count; ++index )
		{
			native.GSShaderResources[index] = m_GSShaderResources[index];
		}
		
		for( int index = 0; index < m_GSConstantBuffers->Count; ++index )
		{
			native.GSConstantBuffers[index] = m_GSConstantBuffers[index];
		}
		
		for( int index = 0; index < m_PSSamplers->Count; ++index )
		{
			native.PSSamplers[index] = m_PSSamplers[index];
		}
		
		for( int index = 0; index < m_PSShaderResources->Count; ++index )
		{
			native.PSShaderResources[index] = m_PSShaderResources[index];
		}
		
		for( int index = 0; index < m_PSConstantBuffers->Count; ++index )
		{
			native.PSConstantBuffers[index] = m_PSConstantBuffers[index];
		}
		
		for( int index = 0; index < m_IAVertexBuffers->Count; ++index )
		{
			native.IAVertexBuffers[index] = m_IAVertexBuffers[index];
		}
		
		return native;
	}
	
	System::Object^ StateBlockMask::Clone2()
	{
		return Clone();
	}
	
	bool StateBlockMask::VertexShader::get()
	{
		return m_VS;
	}
	
	void StateBlockMask::VertexShader::set( bool value )
	{
		m_VS = value;
	}
	
	ReadOnlyCollection<bool>^ StateBlockMask::VertexShaderSamplers::get() 
	{
		return gcnew ReadOnlyCollection<bool>( m_VSSamplers );
	}
	
	ReadOnlyCollection<bool>^ StateBlockMask::VertexShaderResources::get() 
	{
		return gcnew ReadOnlyCollection<bool>( m_VSShaderResources );
	}
	
	ReadOnlyCollection<bool>^ StateBlockMask::VertexShaderConstantBuffers::get() 
	{
		return gcnew ReadOnlyCollection<bool>( m_VSConstantBuffers );
	}
	
	bool StateBlockMask::GeometryShader::get()
	{
		return m_GS;
	}
	
	void StateBlockMask::GeometryShader::set( bool value )
	{
		m_GS = value;
	}
	
	ReadOnlyCollection<bool>^ StateBlockMask::GeometryShaderSamplers::get() 
	{
		return gcnew ReadOnlyCollection<bool>( m_GSSamplers );
	}
	
	ReadOnlyCollection<bool>^ StateBlockMask::GeometryShaderResources::get() 
	{
		return gcnew ReadOnlyCollection<bool>( m_GSShaderResources );
	}
	
	ReadOnlyCollection<bool>^ StateBlockMask::GeometryShaderConstantBuffers::get() 
	{
		return gcnew ReadOnlyCollection<bool>( m_GSConstantBuffers );
	}
	
	bool StateBlockMask::PixelShader::get()
	{
		return m_PS;
	}
	
	void StateBlockMask::PixelShader::set( bool value )
	{
		m_PS = value;
	}
	
	ReadOnlyCollection<bool>^ StateBlockMask::PixelShaderSamplers::get() 
	{
		return gcnew ReadOnlyCollection<bool>( m_PSSamplers );
	}
	
	ReadOnlyCollection<bool>^ StateBlockMask::PixelShaderResources::get() 
	{
		return gcnew ReadOnlyCollection<bool>( m_PSShaderResources );
	}
	
	ReadOnlyCollection<bool>^ StateBlockMask::PixelShaderConstantBuffers::get() 
	{
		return gcnew ReadOnlyCollection<bool>( m_PSConstantBuffers );
	}
	
	ReadOnlyCollection<bool>^ StateBlockMask::VertexBuffers::get() 
	{
		return gcnew ReadOnlyCollection<bool>( m_IAVertexBuffers );
	}
	
	bool StateBlockMask::IndexBuffer::get()
	{
		return m_IAIndexBuffer;
	}
	
	void StateBlockMask::IndexBuffer::set( bool value )
	{
		m_IAIndexBuffer = value;
	}
	
	bool StateBlockMask::InputLayout::get()
	{
		return m_IAInputLayout;
	}
	
	void StateBlockMask::InputLayout::set( bool value )
	{
		m_IAInputLayout = value;
	}
	
	bool StateBlockMask::PrimitiveTopology::get()
	{
		return m_IAPrimitiveTopology;
	}
	
	void StateBlockMask::PrimitiveTopology::set( bool value )
	{
		m_IAPrimitiveTopology = value;
	}
	
	bool StateBlockMask::RenderTargets::get()
	{
		return m_OMRenderTargets;
	}
	
	void StateBlockMask::RenderTargets::set( bool value )
	{
		m_OMRenderTargets = value;
	}
	
	bool StateBlockMask::DepthStencilState::get()
	{
		return m_OMDepthStencilState;
	}
	
	void StateBlockMask::DepthStencilState::set( bool value )
	{
		m_OMDepthStencilState = value;
	}
	
	bool StateBlockMask::BlendState::get()
	{
		return m_OMBlendState;
	}
	
	void StateBlockMask::BlendState::set( bool value )
	{
		m_OMBlendState = value;
	}
	
	bool StateBlockMask::Viewports::get()
	{
		return m_RSViewports;
	}
	
	void StateBlockMask::Viewports::set( bool value )
	{
		m_RSViewports = value;
	}
	
	bool StateBlockMask::ScissorRectangles::get()
	{
		return m_RSScissorRects;
	}
	
	void StateBlockMask::ScissorRectangles::set( bool value )
	{
		m_RSScissorRects = value;
	}
	
	bool StateBlockMask::RasterizerState::get()
	{
		return m_RSRasterizerState;
	}
	
	void StateBlockMask::RasterizerState::set( bool value )
	{
		m_RSRasterizerState = value;
	}
	
	bool StateBlockMask::StreamOutputBuffers::get()
	{
		return m_SOBuffers;
	}
	
	void StateBlockMask::StreamOutputBuffers::set( bool value )
	{
		m_SOBuffers = value;
	}
	
	bool StateBlockMask::Predication::get()
	{
		return m_Predication;
	}
	
	void StateBlockMask::Predication::set( bool value )
	{
		m_Predication = value;
	}
	
	StateBlockMask^ StateBlockMask::Clone()
	{
		return gcnew StateBlockMask( this );
	}
	
	bool StateBlockMask::operator == ( StateBlockMask^ left, StateBlockMask^ right )
	{
		return StateBlockMask::Equals( left, right );
	}

	bool StateBlockMask::operator != ( StateBlockMask^ left, StateBlockMask^ right )
	{
		return !StateBlockMask::Equals( left, right );
	}

	int StateBlockMask::GetHashCode()
	{
		return m_VS.GetHashCode() + 
			m_VSSamplers->GetHashCode() +
			m_VSShaderResources->GetHashCode() +
			m_VSConstantBuffers->GetHashCode() +
			m_GS.GetHashCode() +
			m_GSSamplers->GetHashCode() +       
			m_GSShaderResources->GetHashCode() +
			m_GSConstantBuffers->GetHashCode() +
			m_PS.GetHashCode() +
			m_PSSamplers->GetHashCode() +       
			m_PSShaderResources->GetHashCode() +
			m_PSConstantBuffers->GetHashCode() +
			m_IAVertexBuffers->GetHashCode() +  
			m_IAIndexBuffer.GetHashCode() +
			m_IAInputLayout.GetHashCode() +
			m_IAPrimitiveTopology.GetHashCode() +
			m_OMRenderTargets.GetHashCode() +
			m_OMDepthStencilState.GetHashCode() +
			m_OMBlendState.GetHashCode() +
			m_RSViewports.GetHashCode() +
			m_RSScissorRects.GetHashCode() +
			m_RSRasterizerState.GetHashCode() +
			m_SOBuffers.GetHashCode() +
			m_Predication.GetHashCode();
	}

	bool StateBlockMask::Equals( Object^ value )
	{
		if( value == nullptr )
			return false;

		if( value->GetType() != GetType() )
			return false;

		return Equals( safe_cast<StateBlockMask^>( value ) );
	}

	bool StateBlockMask::Equals( StateBlockMask^ value )
	{
		return m_VS == value->m_VS && 
			Utilities::CheckElementEquality( m_VSSamplers, value->m_VSSamplers ) &&
			Utilities::CheckElementEquality( m_VSShaderResources, value->m_VSShaderResources ) &&
			Utilities::CheckElementEquality( m_VSConstantBuffers,value->m_VSConstantBuffers ) &&
			m_GS == value->m_GS &&
			Utilities::CheckElementEquality( m_GSSamplers, value->m_GSSamplers ) &&
			Utilities::CheckElementEquality( m_GSShaderResources, value->m_GSShaderResources ) &&
			Utilities::CheckElementEquality( m_GSConstantBuffers, value->m_GSConstantBuffers ) &&
			m_PS == value->m_PS &&
			Utilities::CheckElementEquality( m_PSSamplers, value->m_PSSamplers ) &&
			Utilities::CheckElementEquality( m_PSShaderResources, value->m_PSShaderResources ) &&
			Utilities::CheckElementEquality( m_PSConstantBuffers, value->m_PSConstantBuffers ) &&
			Utilities::CheckElementEquality( m_IAVertexBuffers, value->m_IAVertexBuffers ) &&
			m_IAIndexBuffer == value->m_IAIndexBuffer &&
			m_IAInputLayout == value->m_IAInputLayout &&
			m_IAPrimitiveTopology == value->m_IAPrimitiveTopology &&
			m_OMRenderTargets == value->m_OMRenderTargets &&
			m_OMDepthStencilState == value->m_OMDepthStencilState &&
			m_OMBlendState == value->m_OMBlendState &&
			m_RSViewports == value->m_RSViewports &&
			m_RSScissorRects == value->m_RSScissorRects &&
			m_RSRasterizerState == value->m_RSRasterizerState &&
			m_SOBuffers == value->m_SOBuffers &&
			m_Predication == value->m_Predication;
	}

	bool StateBlockMask::Equals( StateBlockMask^ value1, StateBlockMask^ value2 )
	{
		return value1->m_VS == value2->m_VS && 
			Utilities::CheckElementEquality( value1->m_VSSamplers, value2->m_VSSamplers ) &&
			Utilities::CheckElementEquality( value1->m_VSShaderResources, value2->m_VSShaderResources ) &&
			Utilities::CheckElementEquality( value1->m_VSConstantBuffers,value2->m_VSConstantBuffers ) &&
			value1->m_GS == value2->m_GS &&
			Utilities::CheckElementEquality( value1->m_GSSamplers, value2->m_GSSamplers ) &&
			Utilities::CheckElementEquality( value1->m_GSShaderResources, value2->m_GSShaderResources ) &&
			Utilities::CheckElementEquality( value1->m_GSConstantBuffers, value2->m_GSConstantBuffers ) &&
			value1->m_PS == value2->m_PS &&
			Utilities::CheckElementEquality( value1->m_PSSamplers, value2->m_PSSamplers ) &&
			Utilities::CheckElementEquality( value1->m_PSShaderResources, value2->m_PSShaderResources ) &&
			Utilities::CheckElementEquality( value1->m_PSConstantBuffers, value2->m_PSConstantBuffers ) &&
			Utilities::CheckElementEquality( value1->m_IAVertexBuffers, value2->m_IAVertexBuffers ) &&
			value1->m_IAIndexBuffer == value2->m_IAIndexBuffer &&
			value1->m_IAInputLayout == value2->m_IAInputLayout &&
			value1->m_IAPrimitiveTopology == value2->m_IAPrimitiveTopology &&
			value1->m_OMRenderTargets == value2->m_OMRenderTargets &&
			value1->m_OMDepthStencilState == value2->m_OMDepthStencilState &&
			value1->m_OMBlendState == value2->m_OMBlendState &&
			value1->m_RSViewports == value2->m_RSViewports &&
			value1->m_RSScissorRects == value2->m_RSScissorRects &&
			value1->m_RSRasterizerState == value2->m_RSRasterizerState &&
			value1->m_SOBuffers == value2->m_SOBuffers &&
			value1->m_Predication == value2->m_Predication;
	}
}
}

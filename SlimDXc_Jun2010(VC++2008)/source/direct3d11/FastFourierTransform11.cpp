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
#include "stdafx.h"

#include "../stack_array.h"

#include "Direct3D11Exception.h"
#include "DeviceContext11.h"
#include "UnorderedAccessView11.h"
#include "FastFourierTransform11.h"

using namespace System;

namespace SlimDX
{
namespace Direct3D11
{
	FastFourierTransform::FastFourierTransform( DeviceContext^ context, FastFourierTransformDescription description )
	{
		ID3DX11FFT *pointer = NULL;
		D3DX11_FFT_BUFFER_INFO bufferInfo;
		D3DX11_FFT_DESC desc = description.ToUnmanaged();

		HRESULT hr = D3DX11CreateFFT( context->InternalPointer, &desc, 0, &bufferInfo, &pointer );
		if (RECORD_D3D11(hr).IsFailure)
			throw gcnew Direct3D11Exception( Result::Last );

		Construct( pointer );
		bufferRequirements = FastFourierTransformBufferRequirements( bufferInfo );
	}

	FastFourierTransform::FastFourierTransform( DeviceContext^ context, FastFourierTransformDescription description, FastFourierTransformCreationFlags flags )
	{
		ID3DX11FFT *pointer = NULL;
		D3DX11_FFT_BUFFER_INFO bufferInfo;
		D3DX11_FFT_DESC desc = description.ToUnmanaged();

		HRESULT hr = D3DX11CreateFFT( context->InternalPointer, &desc, static_cast<UINT>( flags ), &bufferInfo, &pointer );
		if (RECORD_D3D11(hr).IsFailure)
			throw gcnew Direct3D11Exception( Result::Last );

		Construct( pointer );
		bufferRequirements = FastFourierTransformBufferRequirements( bufferInfo );
	}

	FastFourierTransform^ FastFourierTransform::Create1DComplex( DeviceContext^ context, int x )
	{
		return Create1DComplex( context, x, FastFourierTransformCreationFlags::None );
	}

	FastFourierTransform^ FastFourierTransform::Create1DComplex( DeviceContext^ context, int x, FastFourierTransformCreationFlags flags )
	{
		ID3DX11FFT *pointer = NULL;
		D3DX11_FFT_BUFFER_INFO bufferInfo;

		HRESULT hr = D3DX11CreateFFT1DComplex( context->InternalPointer, x, static_cast<UINT>( flags ), &bufferInfo, &pointer );
		if (RECORD_D3D11(hr).IsFailure)
			return nullptr;

		FastFourierTransform^ result = FastFourierTransform::FromPointer( pointer );
		result->bufferRequirements = FastFourierTransformBufferRequirements( bufferInfo );

		return result;
	}

	FastFourierTransform^ FastFourierTransform::Create1DReal( DeviceContext^ context, int x )
	{
		return Create1DReal( context, x, FastFourierTransformCreationFlags::None );
	}

	FastFourierTransform^ FastFourierTransform::Create1DReal( DeviceContext^ context, int x, FastFourierTransformCreationFlags flags )
	{
		ID3DX11FFT *pointer = NULL;
		D3DX11_FFT_BUFFER_INFO bufferInfo;

		HRESULT hr = D3DX11CreateFFT1DReal( context->InternalPointer, x, static_cast<UINT>( flags ), &bufferInfo, &pointer );
		if (RECORD_D3D11(hr).IsFailure)
			return nullptr;

		FastFourierTransform^ result = FastFourierTransform::FromPointer( pointer );
		result->bufferRequirements = FastFourierTransformBufferRequirements( bufferInfo );

		return result;
	}

	FastFourierTransform^ FastFourierTransform::Create2DComplex( DeviceContext^ context, int x, int y )
	{
		return Create2DComplex( context, x, y, FastFourierTransformCreationFlags::None );
	}

	FastFourierTransform^ FastFourierTransform::Create2DComplex( DeviceContext^ context, int x, int y, FastFourierTransformCreationFlags flags )
	{
		ID3DX11FFT *pointer = NULL;
		D3DX11_FFT_BUFFER_INFO bufferInfo;

		HRESULT hr = D3DX11CreateFFT2DComplex( context->InternalPointer, x, y, static_cast<UINT>( flags ), &bufferInfo, &pointer );
		if (RECORD_D3D11(hr).IsFailure)
			return nullptr;

		FastFourierTransform^ result = FastFourierTransform::FromPointer( pointer );
		result->bufferRequirements = FastFourierTransformBufferRequirements( bufferInfo );

		return result;
	}

	FastFourierTransform^ FastFourierTransform::Create2DReal( DeviceContext^ context, int x, int y )
	{
		return Create2DReal( context, x, y, FastFourierTransformCreationFlags::None );
	}

	FastFourierTransform^ FastFourierTransform::Create2DReal( DeviceContext^ context, int x, int y, FastFourierTransformCreationFlags flags )
	{
		ID3DX11FFT *pointer = NULL;
		D3DX11_FFT_BUFFER_INFO bufferInfo;

		HRESULT hr = D3DX11CreateFFT2DReal( context->InternalPointer, x, y, static_cast<UINT>( flags ), &bufferInfo, &pointer );
		if (RECORD_D3D11(hr).IsFailure)
			return nullptr;

		FastFourierTransform^ result = FastFourierTransform::FromPointer( pointer );
		result->bufferRequirements = FastFourierTransformBufferRequirements( bufferInfo );

		return result;
	}

	FastFourierTransform^ FastFourierTransform::Create3DComplex( DeviceContext^ context, int x, int y, int z )
	{
		return Create3DComplex( context, x, y, z, FastFourierTransformCreationFlags::None );
	}

	FastFourierTransform^ FastFourierTransform::Create3DComplex( DeviceContext^ context, int x, int y, int z, FastFourierTransformCreationFlags flags )
	{
		ID3DX11FFT *pointer = NULL;
		D3DX11_FFT_BUFFER_INFO bufferInfo;

		HRESULT hr = D3DX11CreateFFT3DComplex( context->InternalPointer, x, y, z, static_cast<UINT>( flags ), &bufferInfo, &pointer );
		if (RECORD_D3D11(hr).IsFailure)
			return nullptr;

		FastFourierTransform^ result = FastFourierTransform::FromPointer( pointer );
		result->bufferRequirements = FastFourierTransformBufferRequirements( bufferInfo );

		return result;
	}

	FastFourierTransform^ FastFourierTransform::Create3DReal( DeviceContext^ context, int x, int y, int z )
	{
		return Create3DReal( context, x, y, z, FastFourierTransformCreationFlags::None );
	}

	FastFourierTransform^ FastFourierTransform::Create3DReal( DeviceContext^ context, int x, int y, int z, FastFourierTransformCreationFlags flags )
	{
		ID3DX11FFT *pointer = NULL;
		D3DX11_FFT_BUFFER_INFO bufferInfo;

		HRESULT hr = D3DX11CreateFFT3DReal( context->InternalPointer, x, y, z, static_cast<UINT>( flags ), &bufferInfo, &pointer );
		if (RECORD_D3D11(hr).IsFailure)
			return nullptr;

		FastFourierTransform^ result = FastFourierTransform::FromPointer( pointer );
		result->bufferRequirements = FastFourierTransformBufferRequirements( bufferInfo );

		return result;
	}

	float FastFourierTransform::ForwardScale::get()
	{
		return InternalPointer->GetForwardScale();
	}

	void FastFourierTransform::ForwardScale::set( float value )
	{
		HRESULT hr = InternalPointer->SetForwardScale( value );
		RECORD_D3D11( hr );
	}

	float FastFourierTransform::InverseScale::get()
	{
		return InternalPointer->GetInverseScale();
	}

	void FastFourierTransform::InverseScale::set( float value )
	{
		HRESULT hr = InternalPointer->SetInverseScale( value );
		RECORD_D3D11( hr );
	}

	Result FastFourierTransform::AttachBuffersAndPrecompute( array<UnorderedAccessView^>^ tempBuffers, array<UnorderedAccessView^>^ precomputeBuffers )
	{
		if (tempBuffers == nullptr)
			throw gcnew ArgumentNullException("temporaryBuffers");
		if (precomputeBuffers == nullptr)
			throw gcnew ArgumentNullException("precomputeBuffers");

		if (tempBuffers->Length == 0)
			throw gcnew ArgumentOutOfRangeException("temporaryBuffers");
		if (precomputeBuffers->Length == 0)
			throw gcnew ArgumentOutOfRangeException("precomputeBuffers");

		stack_array<ID3D11UnorderedAccessView*> nativeTempBuffers = stackalloc(ID3D11UnorderedAccessView*, tempBuffers->Length);
		stack_array<ID3D11UnorderedAccessView*> nativePrecomputeBuffers = stackalloc(ID3D11UnorderedAccessView*, precomputeBuffers->Length);

		for (int i = 0; i < tempBuffers->Length; i++)
			nativeTempBuffers[i] = tempBuffers[i] == nullptr ? NULL : tempBuffers[i]->InternalPointer;

		for (int i = 0; i < precomputeBuffers->Length; i++)
			nativePrecomputeBuffers[i] = precomputeBuffers[i] == nullptr ? NULL : precomputeBuffers[i]->InternalPointer;

		HRESULT hr = InternalPointer->AttachBuffersAndPrecompute( tempBuffers->Length, &nativeTempBuffers[0], precomputeBuffers->Length, &nativePrecomputeBuffers[0] );
		return RECORD_D3D11( hr );
	}

	Result FastFourierTransform::ForwardTransform( UnorderedAccessView^ input, UnorderedAccessView^ output )
	{
		if ( output == nullptr )
			throw gcnew System::ArgumentNullException( "output" );

		ID3D11UnorderedAccessView* inputBuffer = input == nullptr ? NULL : input->InternalPointer;
		ID3D11UnorderedAccessView* outputBuffer = output->InternalPointer;

		HRESULT hr = InternalPointer->ForwardTransform( inputBuffer, &outputBuffer );
		return RECORD_D3D11( hr );
	}

	UnorderedAccessView^ FastFourierTransform::ForwardTransform( UnorderedAccessView^ input )
	{
		ID3D11UnorderedAccessView* inputBuffer = input == nullptr ? NULL : input->InternalPointer;
		ID3D11UnorderedAccessView* outputBuffer = NULL;

		HRESULT hr = InternalPointer->ForwardTransform( inputBuffer, &outputBuffer );
		if (RECORD_D3D11(hr).IsFailure)
			return nullptr;

		return UnorderedAccessView::FromPointer( outputBuffer );
	}

	Result FastFourierTransform::InverseTransform( UnorderedAccessView^ input, UnorderedAccessView^ output )
	{
		if ( output == nullptr )
			throw gcnew System::ArgumentNullException( "output" );

		ID3D11UnorderedAccessView* inputBuffer = input == nullptr ? NULL : input->InternalPointer;
		ID3D11UnorderedAccessView* outputBuffer = output->InternalPointer;

		HRESULT hr = InternalPointer->InverseTransform( inputBuffer, &outputBuffer );
		return RECORD_D3D11( hr );
	}

	UnorderedAccessView^ FastFourierTransform::InverseTransform( UnorderedAccessView^ input )
	{
		ID3D11UnorderedAccessView* inputBuffer = input == nullptr ? NULL : input->InternalPointer;
		ID3D11UnorderedAccessView* outputBuffer = NULL;

		HRESULT hr = InternalPointer->InverseTransform( inputBuffer, &outputBuffer );
		if (RECORD_D3D11(hr).IsFailure)
			return nullptr;

		return UnorderedAccessView::FromPointer( outputBuffer );
	}
}
}

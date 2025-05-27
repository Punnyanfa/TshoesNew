import { instance } from "./api-instance-provider";



export async function getOrder() {
  try {
    const response = await instance.get(`/Order/${id}`);
    if (response.data.code === 200) {
      return response.data.data;  // Trả về data của sản phẩm cụ thể
    }
    throw new Error(response.data.message || `Failed to fetch product with id ${id}`);
  } catch (error) {
    console.error(`Error getting product with id ${id}:`, error);
    throw error;
  }
}

// Get product by ID
export async function getOrderById(id) {
  try {
    const response = await instance.get(`/Order`);
    if (response.data.code === 200) {
      return response.data.data;  // Trả về data của sản phẩm cụ thể
    }
    throw new Error(response.data.message || `Failed to fetch order with id ${id}`);
  } catch (error) {
    console.error(`Error getting order with id ${id}:`, error);
    throw error;
  }
}

// Post order
export async function postOrder(orderData) {
  console.log('Sending order data:', orderData);
  try {
    const response = await instance.post('/Order', orderData);
    console.log('Raw API response:', response);

    // If we have a successful response
    if (response.data) {
      // Check if we have a payment URL in the response data
      if (response.data.data && response.data.data.paymentUrl) {
        return {
          code: response.data.code,
          paymentUrl: response.data.data.paymentUrl
        };
      }
      // Return the full response data if no payment URL
      return response.data;
    }

    throw new Error('No response data received from server');
  } catch (error) {
    console.error('Error creating order:', error.response?.data || error);
    throw error.response?.data || error;
  }
}

export async function getAllOrders() {
  try {
    const response = await instance.get('/Order');
    if (response.data.code === 200) {
      return response.data.data;
    }
    throw new Error(response.data.message || 'Failed to fetch orders');
  } catch (error) {
    console.error('Error getting orders:', error);
    throw error;
  }
}


export async function putPaymentWebhook(id, status) {
  console.log('id', id);
  console.log('status', status);
  try {
    const response = await instance.put('/Payment/webhook', {
      id,
      status
    });
    if (response.status === 200) {
      return response.data;
    }
    throw new Error(response.data?.message || 'Failed to update payment status');
  } catch (error) {
    console.error('Error updating payment status:', error.response?.data || error);
    throw error.response?.data || error;
  }
}

// Lấy danh sách đơn hàng theo userId
export async function getOrdersByUserId(userId) {
  try {
    const response = await instance.get(`/Order/user/${userId}`);
    if (response.data.code === 200) {
      return response.data.data;
    }
    throw new Error(response.data.message || `Failed to fetch orders for userId ${userId}`);
  } catch (error) {
    console.error(`Error getting orders for userId ${userId}:`, error);
    throw error;
  }
}



import { instance } from "./api-instance-provider";


// Get product by ID
export async function getOrderById(id) {
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

// Post order
export async function postOrder(orderData) {
  try {
    const response = await instance.post('/Order', orderData);
    if (response.data.code === 200) {
      return response.data.data;  // Return the created order data
    }
    throw new Error(response.data.message || 'Failed to create order');
  } catch (error) {
    console.error('Error creating order:', error);
    throw error;
  }
}

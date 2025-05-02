import { getAllOrders as fetchOrders, postOrder } from '~/server/order-service';

export async function getAllOrders() {
  try {
    const response = await fetchOrders();
    return response;
  } catch (error) {
    console.error('Error in getAllOrders:', error);
    throw error;
  }
}

export { postOrder }; 
import { getAllProducts as fetchProducts, updateProductStatus, deleteProduct } from '~/server/product-service';

export async function getAllProducts() {
  try {
    const response = await fetchProducts();
    return response;
  } catch (error) {
    console.error('Error in getAllProducts:', error);
    throw error;
  }
}

export { updateProductStatus, deleteProduct }; 
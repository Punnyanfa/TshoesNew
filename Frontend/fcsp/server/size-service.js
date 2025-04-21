import { instance } from "./api-instance-provider";

/**
 * Get all available sizes
 * @returns {Promise<Array>} Array of size objects
 */
export async function getAllSizes() {
  try {
    const response = await instance.get('/Size');
    
    if (response.data.code === 200) {
      return response.data.data;
    }
    
    throw new Error(response.data.message || 'Failed to fetch sizes');
  } catch (error) {
    console.error('Error fetching sizes:', error);
    throw error;
  }
}

/**
 * Get size ID by size value
 * @param {number} sizeValue - The shoe size value (e.g., 38, 39, 40)
 * @returns {Promise<number|null>} The size ID or null if not found
 */
export async function getSizeIdByValue(sizeValue) {
  try {
    const sizes = await getAllSizes();
    const size = sizes.find(s => s.sizeValue === sizeValue && !s.isDeleted);
    return size ? size.id : null;
  } catch (error) {
    console.error('Error getting size ID:', error);
    throw error;
  }
}

/**
 * Get size value by ID
 * @param {number} sizeId - The size ID
 * @returns {Promise<number|null>} The size value or null if not found
 */
export async function getSizeValueById(sizeId) {
  try {
    const sizes = await getAllSizes();
    const size = sizes.find(s => s.id === sizeId && !s.isDeleted);
    return size ? size.sizeValue : null;
  } catch (error) {
    console.error('Error getting size value:', error);
    throw error;
  }
}

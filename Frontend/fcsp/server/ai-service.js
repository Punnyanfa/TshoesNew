import { instance } from "./api-instance-provider";

class AIService {
  /**
   * Generate image using AI based on prompt and optional image
   * @param {FormData} formData - Form data containing ImageFile, Prompt, OwnerId, and Status
   * @returns {Promise} Response from the API
   */
  async generateImage(formData) {
    try {
      const response = await instance.post('/Texture', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });
      return response.data;
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  /**
   * Get list of generated images
   * @param {Object} params - Query parameters for filtering
   * @returns {Promise} Response from the API
   */
  async getGeneratedImages(params = {}) {
    try {
      const response = await instance.get('/Texture', { params });
      return response.data;
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  /**
   * Get a specific generated image by ID
   * @param {number} id - Image ID
   * @returns {Promise} Response from the API
   */
  async getGeneratedImageById(id) {
    try {
      const response = await instance.get(`/Texture/${id}`);
      return response.data;
    } catch (error) {
      this.handleError(error);
      throw error;
    }
  }

  /**
   * Handle API errors
   * @param {Error} error - Error object
   * @private
   */
  handleError(error) {
    if (error.response) {
      // The request was made and the server responded with a status code
      // that falls out of the range of 2xx
      console.error('API Error Response:', error.response.data);
      console.error('Status:', error.response.status);
    } else if (error.request) {
      // The request was made but no response was received
      console.error('No response received:', error.request);
    } else {
      // Something happened in setting up the request that triggered an Error
      console.error('Error:', error.message);
    }
  }
}

// Create a singleton instance
const aiService = new AIService();

export default aiService;

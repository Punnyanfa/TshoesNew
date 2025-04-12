import { instance } from "./api-instance-provider";


export async function getAllUser() {
    try {
      const response = await instance.get(`/Auth`);
      if (response.data.code === 200) {
        return response.data.data.users;  // Return the users array from the response
      }
      throw new Error(response.data.message || `Failed to fetch users`);
    } catch (error) {
      console.error(`Error getting users:`, error);
      throw error;
    }
  }
  
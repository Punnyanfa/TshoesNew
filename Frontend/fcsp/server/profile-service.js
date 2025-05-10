import { instance } from "./api-instance-provider";
export async function getProfile(id) {
    try {
      const response = await instance.get(`/Auth/${id}`);
      if (response.data.code === 200) {
        return response.data.data;  // Trả về data của user
      }
      throw new Error(response.data.message || `Failed to fetch user with id ${id}`);
    } catch (error) {
      console.error(`Error getting user with id ${id}:`, error);
      throw error;
    }
  }

// Thêm function updateProfile
export async function updateProfile(profile) {
    console.log(profile);
  try {
    const response = await instance.put('/Auth/information', profile);
    return response.data;
  } catch (error) {
    console.error('Error updating profile:', error);
    throw error;
  }
}
import { instance } from "./api-instance-provider";

export async function getMyCustom(userId) {
  try {
    const response = await instance.get(`/CustomShoeDesign/user/${userId}`);
    return response.data;
  } catch (error) {
    console.error(`Error getting my custom:`, error);
    throw error;
  }
}
export async function getMyCustomById(id) {
  try {
    const response = await instance.get(`/CustomShoeDesign/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error getting my custom by id:`, error);
    throw error;
  }
}
export async function deleteCustom(id) {
  try {
    const response = await instance.delete(`/CustomShoeDesign/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error deleting custom:`, error);
    throw error;
  }
}
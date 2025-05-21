import { instance } from "./api-instance-provider";

export async function getMyCustom() {
  try {
    const response = await instance.get(`/CustomShoeDesign/user/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error getting my custom:`, error);
    throw error;
  }
}
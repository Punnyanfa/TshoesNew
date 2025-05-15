import { instance } from "./api-instance-provider";

export async function getManufacture() {
  try {
    const response = await instance.get(`/service`);
    return response.data;
  } catch (error) {
    console.error(`Error getting manufacture:`, error);
    throw error;
  }
}

export async function addManufacture(manufacture) {
  console.log(manufacture);
  try {
    const response = await instance.post(`/service`, manufacture);
    return response.data;   
  } catch (error) {
    console.error(`Error adding manufacture:`, error);
    throw error;
  }
}
export async function getManufacturerById(id) {
  try {
    const response = await instance.get(`/Manufacturer/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error getting manufacture:`, error);
    throw error;
  }
}
export async function getManufacturerAll() {
  try {
    const response = await instance.get(`/Manufacturer`);
    return response.data;
  } catch (error) {
    console.error(`Error getting manufacture:`, error);
    throw error;
  }
}



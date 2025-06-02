import { instance } from "./api-instance-provider";

export async function getUserBalance(userId) {
  try {
    // Assuming the API endpoint to get user balance is /user/balance/{userId}
    const response = await instance.get(`/user/${userId}/balance`);
    if (response.data.code === 200) {
      return response.data.data.balance; // Assuming the response data has a 'balance' field
    }
    throw new Error(response.data.message || `Failed to fetch user balance`);
  } catch (error) {
    console.error(`Error getting user balance:`, error);
    // Return a default value or throw the error, depending on desired behavior
    throw error; 
  }
} 
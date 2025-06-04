import { instance } from "./api-instance-provider";



export async function getPayment() {
  try {
    const response = await instance.get(`/Payment`);
    if (response.data.code === 200) {
      return response.data.data;  
    }
    throw new Error(response.data.message || `Failed to fetch payment`);
  } catch (error) {
    console.error(`Error getting payment:`, error);
    throw error;
  }
}

export async function rechargePayment({ userId, paymentId, amount }) {
  try {
    const response = await instance.post(`/Payment/recharge`, {
      userId,
      paymentId,
      amount
    });
    if (response.data.code === 200) {
      return response.data.data;
    }
    throw new Error(response.data.message || `Failed to recharge payment`);
  } catch (error) {
    console.error(`Error recharging payment:`, error);
    throw error;
  }
}

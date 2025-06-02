import { instance } from "./api-instance-provider";

export async function getBalance(id) {
    try {
        const response = await instance.get(`Auth/balance/${id}`);
        return response.data;
    } catch (error) {
        console.error('Error getting balance:', error);
        throw error;
    }
}
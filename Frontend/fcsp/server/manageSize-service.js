import { instance } from "./api-instance-provider";


export async function getAllSizes() {
    try {
        const response = await instance.get('/Size');
        return response.data;
    } catch (error) {
        console.error('Error getting all sizes:', error);
        throw error;
    }
}
export async function createSizes(size) {
    try {
        const response = await instance.post('/Size', size);
        return response.data;
    } catch (error) {
        console.error('Error creating size:', error);
        throw error;
    }
}
export async function DeletedSize(id) {
    try {
        const response = await instance.delete(`/Size/${id}`);
        return response.data;
    } catch (error) {
        console.error('Error deleting size:', error);
        throw error;
    }
}

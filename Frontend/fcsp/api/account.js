import { getAllUser, updateStatus } from '~/server/manageAccounts-service';

export async function getAllAccounts() {
  try {
    const response = await getAllUser();
    return response;
  } catch (error) {
    console.error('Error in getAllAccounts:', error);
    throw error;
  }
}

export { updateStatus }; 
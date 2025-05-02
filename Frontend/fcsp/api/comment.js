import { getAllRating } from '~/server/rating-service';

export async function getAllComments() {
  try {
    const response = await getAllRating();
    return response;
  } catch (error) {
    console.error('Error in getAllComments:', error);
    throw error;
  }
} 
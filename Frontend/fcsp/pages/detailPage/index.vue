<script setup>
import { onMounted, ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();
const product = ref(null);

const fetchProduct = () => {
  const productId = route.params.id;
  product.value = {
    id: productId,
    name: `Product ${productId}`,
    description: `Detailed description of product ${productId}`,
    price: (100 + parseInt(productId)).toFixed(2),
    image: 'https://heramo.com/blog/wp-content/uploads/2023/09/custom-giay-la-gi-5.jpg',
  };
};

onMounted(fetchProduct);
watch(() => route.params.id, fetchProduct); // Cập nhật nếu route thay đổi
</script>

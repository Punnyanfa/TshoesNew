<script setup>
import { ref } from 'vue';
import { ShoppingCartOutlined } from '@ant-design/icons-vue';
import { useCartStore } from '~/store/cart';

const props = defineProps({
  product: {
    type: Object,
    required: true
  }
});

const cartStore = useCartStore();

const addToCart = () => {
  cartStore.addItem(props.product);
};
</script> 
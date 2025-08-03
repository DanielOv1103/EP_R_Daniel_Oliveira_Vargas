import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      // para que en tiempo de build, @/ apunte a src/
      '@': path.resolve(__dirname, 'src')
    }
  }
});

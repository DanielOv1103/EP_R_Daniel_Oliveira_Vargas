import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite';
import path from 'path';

export default defineConfig({
  plugins: [react(), tailwindcss() ],
  resolve: {
    alias: {
      // para que en tiempo de build, @/ apunte a src/
      '@': path.resolve(__dirname, 'src')
    }
  }
});

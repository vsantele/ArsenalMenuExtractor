import { defineConfig } from "astro/config"

import mdx from "@astrojs/mdx"

// https://astro.build/config
import sitemap from "@astrojs/sitemap"
import pandacss from "@pandacss/astro"

// https://astro.build/config
export default defineConfig({
  site: "https://arsenalmenu.vsantele.dev/",
  integrations: [mdx(), sitemap(), pandacss()],
})

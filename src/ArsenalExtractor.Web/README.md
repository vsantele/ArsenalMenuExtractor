# Accessible Astro Starter

This starter project is build upon the (awesome) [Astro Static Site Builder](https://astro.build/). To help you build your project *faster*, this theme includes some dedicated accessible components (such as a keyboard accessible and responsive navigation) and several components coming from the [Accessible Astro Components](https://github.com/markteekman/accessible-astro-components) npm package. This theme also includes example pages, a blog with dynamic routes, a custom 404 page and many **Design System** utility classes, patterns and primatives (such as grids, buttons, lists, spacings, sizes and more).

![accessible-astro-starter](https://user-images.githubusercontent.com/3909046/203257395-163f7002-65ff-4c55-a00c-256d708f6b2d.png)

[Live demo](https://starter.accessible-astro.dev)

## Getting started
Run the following commands in your project folder to get started:
```console
npm install && npm start
```

## Accessibility features

- Accessible landmarks such as `header`, `main`, `footer`, `section` and `nav`
- Outline focus indicator which works on dark and light backgrounds
- Several `aria` attributes which provide a better experience for screen reader users
- `SkipLinks.astro` component to skip to either the main menu or the main content *
- `Navigation.astro` component with keyboard accessible (dropdown) navigation (arrow keys, escape key)
- `ResponsiveToggle.astro` component with an accessible responsive toggle button for the mobile navigation
- `DarkMode.astro` component toggle with accessible button and a user system preferred color scheme *
- `Breadcrumbs.astro` component provides a way to navigate the blog posts *
- `Pagination.astro` component provides another way to navigate the blog posts *
- `Header.astro` component included in the `DefaultLayout.astro` layout
- `Footer.astro` component included in the `DefaultLayout.astro` layout
- `.sr-only` utility class for screen reader only text content (hides text visually)
- `prefers-reduced-motion` disables animations for users that have this preference turned on

**Provided by the Accessible Astro Components package.*

## Extra features

- `SiteMeta.astro` SEO component for setting custom meta deta on different pages
- `[...page].astro` and `[post].astro` demonstrate the use of dynamic routes and provide a basic blog with breadcrumbs and pagination
- `404.astro` provides a custom 404 error page which you can adjust to your needs
- [Accessible Astro Components](https://github.com/markteekman/accessible-astro-components) package comes preinstalled with extra components such as Accordions, Modals and Notifications
- Nate Moore's awesome [Astro Icon](https://github.com/natemoo-re/astro-icon) package is also preinstalled which can be applied to different use cases

## Utilities

👉🏻 &nbsp;  _Note: Examples are included in the **index.astro** file_

This starter contains some extra **Design System** like utility classes to aid in the global layout of your project. All of this can be found in the `src/assets/scss/base` directory, and compiles in the `src/assets/globals.scss` file. Using these primitives is totally optional, use or remove at your own preference :) What it contains:

- Reset file to reset browser defaults and ensure everything looks good
- Some basic font settings, such as responsive heading sizes
- Color shades for your color pallet, rendered to the `:root` of your website as custom properties
- Simple auto-grid setting using `display: grid` and a `data-attribute`
- A bit more advanced grid settings using `display` grid to easily setup a 12-column layout
- Utilities set in `_utility.scss` for things such as spacing, sizes, colors, and box-shadows

### 12 Column Grid

12 Column Grid enables you to build custom column based layouts. Using `display: grid;` you can define the amount of columns that elements should take up in your `.grid`, and for which breakpoint they should do so. You can also use offset on a grid to create an extra column of offset. The grid offers different utilities for the individual grid items such `equal-height`, `x-start`, `x-center`, `x-end`, `y-start`, `y-center` and `y-end`. Grid comes with default gutters, to disable them you can use the utility class `no-gutters`.

```html
<div class="container">
  <div class="grid">
    <div class="small-12 medium-3">
      <h3>Sidebar</h3>
      <!-- ... -->
    </div>
    <div class="small-12 medium-8 offset-medium-4">
      <h3>Main</h3>
      <!-- ... -->
    </div>
  </div>
</div>
```

### Alignment

Alignment classes can be used to align elements in your HTML. There's `align-center`, `align-horizontal` and `align-vertical`.

```html
<div class="align-center w-screen h-screen">
  <h1>Center me on the screen!</h1>
</div>
```

### Animations

There are a couple of basic animations which you can throw on your HTML elements like `blink`, `fade-in`, `pop-up` and `spin`. You can also use some animation delays to create different effects.

```html
<div data-animation="fade-in">
  <p>I've got a fancy fade-in animation that starts right away.</p>
</div>
<div data-animation="fade-in" data-animation-delay="0.25s">
  <p>I've got a fancy fade-in animation after 0.25s.</p>
</div>
<div data-animation="fade-in" data-animation-delay="0.5s">>
  <p>I've got a fancy fade-in animation after 0.5s.</p>
</div>
```

### Auto Grid

Simply apply the `data-auto-grid` attribute on your parent `div` with a number from 2-6 (if you need more columns just tweak the for loop in `_auto-grid.scss`). The grid automatically creates new rows (this is how `display: grid` works by default).

```html
<div class="container" data-auto-grid="3">
  <p>First column</p>
  <p>Second column</p>
  <p>Third column</p>
</div>
```

You can also center the contents of the cell using the `data-grid-center` attribute. Use it together with `data-auto-grid`.

```html
<div class="container" data-auto-grid="3" data-grid-center>
  <!-- ... -->
</div>
```

### Borders

With two border radius utility classes (`radius-small` and `radius-large`) you can change the border radius of your HTML elements on the fly.

```html
<div class="bg-neutral-900 space-32 radius-large">
  <p>Lorem ipsum dolor sit amet consectetur adipisicing elit.</p>
</div>
```

### Buttons

A Button primitive to easily apply button styles to your `<button>` and `<a>` tags. Simply apply the class `button` to your element. There are different variations in colors (`color-secondary`, `color-info`, `color-success`, `color-warning`, `color-error`) which is primary by default, sizing (`size-tiny`, `size-large`, `size-huge`) which is medium by default and behavior (`behavior-full`) which stretches the button to 100% width.

```html
<a href="/" class="button color-success size-huge behavior-full">
  Click me to go to space!
</a>
```

### Colors

You can setup your own color schemes in the `_colors.scss` file. You'll find a SCSS map, which gets printed inside `_root.scss` as custom properties. There are also several color utilities such as `text-primary-#` and `bg-neutral-#` based on all colors you've defined. `text-primary-#` should be used on a parent element to give the child's the respective color.

```scss
$colors: (
  primary: (
    100: hsl(262, 90%, 95%),
    200: hsl(262, 100%, 88%),
    300: hsl(262, 100%, 78%),
    400: hsl(268, 82%, 60%),
    500: hsl(273, 79%, 48%),
  ),
  // ...
);
```

```html
<div class="text-neutral-100 bg-neutral-900 space-32">
  <p>Dark background with white text on it!</p>
</div>
```

### Elevations

Use elevations on your HTML elements to add a box shadow of different intensities. Use either `elevation-100`, `elevation-200`, `elevation-300`, `elevation-400` or `elevation-500`.

```html
<div class="space-32 radius-large elevation-400">
  <p>Lorem ipsum dolor sit amet consectetur adipisicing elit.</p>
</div>
```

### Height / Width

You can set the height and width of your elements to either 100 view port units or percentages using `h-screen`, `h-full`, `w-screen` and `w-full` respectfully.

```html
<div class="h-screen w-screen align-center">
  <p>I'm centered in the middle of the screen!</p>
</div>
```

### Margins, Paddings and Spaces

To prevent spacing each element in your website individually and to prevent inconsistencies, you can use the `.margin-#` and `.padding-#` utility classes. A good practice is to set in on your `<section>` elements, for starters. To space out content you can use a special `.space-content` class on your parent div (for example an element of your `.grid`). By default, margin/padding top and bottom are set with these utilities. You can also set it explicitly using either `.top` of `.bottom` class tokens. Find or tweak all spacing options in `_space.scss`. `space-#` tokens however add padding all around an element, which is great of you need to make card like blocks on the fly by combining `space` with the other utilities.

```html
<section class="padding-32">
  <div class="container">
    <h2 class="margin-16 bottom">Grid example</h2>
  </div>
</section>
```

```html
<div class="box space-32 bg-primary-200 radius-small elevation-100">
  <div class="space-content">
    <h3>I look like a card!</h3>
    <p>And space-content adds space between us for readability.</p>
  </div>
</div>
```

### Sizes

If you need an exception on your font-size for a specific reason you can use size utility classes to accomplish that. Using it is easy. Find or tweak all spacing options in `_size.scss`.

```html
<div class="container">
  <p class="size-20">I'm a bigger font size!</p>
</div>
```

## Other Accessible Astro projects

- [Accessible Astro Dashboard](https://github.com/markteekman/accessible-astro-dashboard/)
- [Accessible Astro Components](https://github.com/markteekman/accessible-astro-components/)
- [Accessible Astro Documentation](https://accessible-astro.dev)

## Helping out

If you find that something isn't working right then I'm always happy to hear it to improve this starter! You can contribute in many ways and forms. Let me know by either:

1. [Filing an issue](https://github.com/markteekman/accessible-astro-starter/issues)
2. [Submitting a pull request](https://github.com/markteekman/accessible-astro-starter/pulls)
3. [Starting a discussion](https://github.com/markteekman/accessible-astro-starter/discussions)
4. [Buying me a coffee!](https://www.buymeacoffee.com/markteekman)

## Thank you!

A big thank you to the creators of the awesome Astro static site generator and to all using this starter to make the web a bit more accessible for all people around the world :)

[![buymeacoffee-button](https://user-images.githubusercontent.com/3909046/150683481-be070424-7bb0-4dd7-a3cb-43b5605163f5.png)](https://www.buymeacoffee.com/markteekman)

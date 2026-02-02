<script lang="ts">
	import { env } from '$env/dynamic/public';
	import LoginForm from '$lib/shadcn/components/login-form.svelte';
	import GalleryVerticalEndIcon from '@lucide/svelte/icons/gallery-vertical-end';
	import { Button } from '$lib/shadcn/components/ui/button/index.js';
	import * as Card from '$lib/shadcn/components/ui/card/index.js';
	import {
		FieldGroup,
		Field,
		FieldLabel,
		FieldDescription,
		FieldSeparator
	} from '$lib/shadcn/components/ui/field/index.js';
	import { Input } from '$lib/shadcn/components/ui/input/index.js';
	import { cn } from '$lib/shadcn/utils.js';
	import type { HTMLAttributes } from 'svelte/elements';
	import type { LayoutProps } from './login/$types';
	import { page } from '$app/state';

	let { children }: LayoutProps = $props();
</script>

<div class="flex min-h-svh flex-col items-center justify-center gap-6 bg-muted p-6 md:p-10">
	<div
		class="flex w-full {page.url.pathname.endsWith('/register')
			? 'max-w-md'
			: 'max-w-sm'} flex-col gap-6 transition-all"
	>
		<a href="##" class="flex items-center gap-2 self-center font-medium">
			<div
				class="flex size-6 items-center justify-center rounded-md bg-primary text-primary-foreground"
			>
				<GalleryVerticalEndIcon class="size-4" />
			</div>
			{env.PUBLIC_APP_TITLE}
		</a>
		<div class="flex flex-col gap-6">
			<Card.Root>
				{@render children()}
			</Card.Root>
			<FieldDescription class="px-6 text-center">
				By clicking continue, you agree to our <a href="##">Terms of Service</a>
				and <a href="##">Privacy Policy</a>.
			</FieldDescription>
		</div>
	</div>
</div>

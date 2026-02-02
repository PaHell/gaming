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
</script>

<Card.Header class="text-center">
	<Card.Title class="text-xl">Oups!</Card.Title>
	<Card.Description>Not again</Card.Description>
</Card.Header>
<Card.Content>
	<form>
		<FieldGroup>
			<Field>
				<FieldLabel for="email">Email</FieldLabel>
				<Input id="email" type="email" placeholder="m@example.com" required />
			</Field>
			<Field>
				<Button type="submit">Request Email</Button>
				<FieldDescription class="text-center">
					Do you remember? <a href="/auth/login">Sign in</a>
				</FieldDescription>
			</Field>
		</FieldGroup>
	</form>
</Card.Content>

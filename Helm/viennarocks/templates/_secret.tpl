{{- define "secret" }}
- name: {{ .key }}
  valueFrom:
    secretKeyRef:
      name: {{ .name }}
      key: {{ .key }}
{{- end }}